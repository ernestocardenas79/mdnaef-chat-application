import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subject, from } from 'rxjs';
import {
  concatMap,
  filter,
  map,
  mergeMap,
  switchMap,
  tap,
} from 'rxjs/operators';
import { environment } from '../../../environment/environment';
import {
  IConversationPh,
  IMessageFromChat,
  IMessagesResponse,
  INewConversationResponse,
  IPostMessage,
  IPostNewConversation,
  IUser,
} from '../interfaces';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private hubConnection: HubConnection;
  private conversationMessage = new Subject<IMessageFromChat>();
  conversationMessage$ = this.conversationMessage.asObservable();

  private newConversation = new Subject<INewConversationResponse>();
  newConversation$ = this.newConversation.asObservable();

  hubConnection$!: Observable<void>;

  contacts = new Map<string, IUser>();

  conversationsPlaceHolder$ = this.authService.userAuthenticated$.pipe(
    tap((r) =>
      this.contacts.set(
        this.authService.logedUser.id!,
        this.authService.logedUser
      )
    ),
    switchMap((user) => this.conversationsPlaceHolder(user as IUser)),
    // tap((c) => console.log('switchMap', c)),
    filter((c) => c.length > 0),
    map((r) =>
      r.map((ph) => ({
        ...ph,
        members: ph.members.filter(
          (m) => m.user !== this.authService.logedUser.user
        ),
      }))
    ),
    tap((ph) => {
      ph.forEach((e) =>
        e.members.map((m) => {
          this.contacts.set(m.id!, m);
        })
      );
    })
    // tap((ph) => console.log('ph', ph))
  );

  constructor(
    private authService: AuthenticationService,
    private httpClient: HttpClient,
    private toastrsrv: ToastrService
  ) {
    console.log('ctor chatservice');
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.urlChat}/chathub`, {
        accessTokenFactory: () => this.authService.token ?? '',
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .build();

    this.hubConnection$ = from(this.hubConnection.start());

    this.hubConnection.on('ReceiveMessage', (user: IUser, message: string) => {
      console.log('ReceiveMessage', { user, message });
      this.conversationMessage.next({
        avatar: user.avatar,
        message,
        name: user.name,
        userid: user.id,
      } as IMessageFromChat);
    });

    this.hubConnection.on(
      'NewConversationPH',
      (nc: INewConversationResponse) => {
        this.newConversation.next(nc);
        this.connectConversation(nc.conversationId);

        this.toastrsrv.info('From: ' + nc.hostUser.name, 'New Message', {
          positionClass: 'toast-bottom-right',
        });
      }
    );

    this.authService.userAuthenticated$
      .pipe(
        concatMap(
          (u) => this.hubConnection$,
          (u) => this.connectUserToHub(u!)
        )
      )
      .subscribe((r) => {
        console.log('SignalR Connected');
        console.log('User Connected:' + this.authService.logedUser.user);
      });

    // this.authService.authentication$
    //   .pipe(filter((u) => u === undefined))
    //   .subscribe((_) =>
    //     this.hubConnection
    //       .stop()
    //       .then((_) => console.log('SignalR connection closed'))
    //   );
  }

  connectUserToHub(user: IUser) {
    console.log('connectionId', this.hubConnection.connectionId);
    return from(this.hubConnection.invoke('UserConnected', user?.user));
  }

  connectConversation(id: string) {
    this.hubConnection
      .invoke('ConnectConversation', id)
      .then(() => console.log('Connected to ' + id))
      .catch((e) => console.error(e));
  }

  notifyConversationCreated(newConversation: INewConversationResponse) {
    this.contacts.set(newConversation.user.id!, newConversation.user);

    this.hubConnection
      .invoke('ConversationCreated', newConversation)
      .then(() => console.log('Connected to ' + newConversation.conversationId))
      .catch((e) => console.error(e));

    this.newConversation.next(newConversation);
  }

  sendMessage(room: string, user: IUser, message: string) {
    return from(
      this.hubConnection.invoke('SendMessage', room, user, message)
    ).pipe(mergeMap(() => this.postMessage(room, user, message)));
  }

  conversationsPlaceHolder(user: IUser) {
    return this.httpClient.get<IConversationPh[]>(
      `${environment.urlChat}/api/conversation/ph`
    );
  }

  conversationMessages(id: string) {
    return this.httpClient.get<IMessagesResponse>(
      `${environment.urlChat}/api/conversation/${id}`
    );
  }

  private postMessage(room: string, user: IUser, message: string) {
    return this.httpClient.post<IPostMessage>(
      `${environment.urlChat}/api/conversation/message`,
      { conversationId: room, member: user.id, message } as IPostMessage
    );
  }

  findUser(user: string) {
    return this.httpClient.get<IUser[]>(
      `${environment.urlChat}/api/user/all/${user}`
    );
  }

  createConversation(user: IUser, hostUser: IUser, message: string) {
    return this.httpClient.post<INewConversationResponse>(
      `${environment.urlChat}/api/conversation/create`,
      { user, hostUser, message } as IPostNewConversation
    );
  }

  testAuthenticationPurpose(userkey: string) {
    if (userkey == 'ma') {
      this.authService.setAuthChange({
        email: 'mario@cardenas.com',
        user: 'mario@cardenas.com',
        name: 'Mario',
        id: '663bcc0e451620cc678b1102',
        avatar: 'https://i.pravatar.cc/40?u=mario@cardenas.comMario',
      });
      return;
    }
    if (userkey == 'ed') {
      this.authService.setAuthChange({
        email: 'edgar@cardenas.com',
        user: 'edgar@cardenas.com',
        name: 'Edgar',
        id: '663cf542117bb7614cbe12a8',
        avatar: 'https://i.pravatar.cc/40?u=edgar@cardenas.comEdgar',
      });
    }
    if (userkey == 'er') {
      this.authService.setAuthChange({
        email: 'ernesto@cardenas.com',
        user: 'ernesto@cardenas.com',
        name: 'Ernesto',
        id: '663bcbdb451620cc678b1101',
        avatar: 'https://i.pravatar.cc/40?u=ernesto@cardenas.comErnesto',
      });
    }
  }
}
