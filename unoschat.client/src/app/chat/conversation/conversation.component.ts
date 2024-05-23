import {
  Component,
  ElementRef,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { Observable, Subject, merge, of } from 'rxjs';
import { map, mergeAll, mergeMap, take, tap } from 'rxjs/operators';
import {
  IConversationPh,
  IMessage,
  IMessagesResponse,
  IUser,
} from '../../core/interfaces';
import { AuthenticationService } from '../../core/services/authentication.service';
import { ChatService } from '../../core/services/chat.service';
import { MessageComponent } from '../message/message.component';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrl: './conversation.component.css',
})
export class ConversationComponent {
  messageResponse!: IMessagesResponse;
  public user: IUser = {} as IUser;

  @ViewChild('text') text!: ElementRef;
  @ViewChild('messagesref', { read: ViewContainerRef })
  messagesref!: ViewContainerRef;

  messagesLoadedSub = new Subject<IMessage[]>();
  messagesLoaded$ = this.messagesLoadedSub.asObservable();
  messages$!: Observable<IMessage[]>;
  firstConversatinonsMessages$ =
    this.chatService.conversationsPlaceHolder$.pipe(
      take(1),
      mergeMap((ph) => this.chatService.conversationMessages(ph[0].id)),
      tap((r) => (this.messageResponse = r)),
      tap((r) => this.chatService.connectConversation(r.conversationId)),
      mergeMap((cm) => of(cm.messages))
    );

  constructor(
    private chatService: ChatService,
    private authService: AuthenticationService
  ) {
    this.chatService.conversationMessage$
      .pipe(map((c) => ({ member: c.userid, message: c.message } as IMessage)))
      .subscribe((m) => this.createMessage(m));

    this.messages$ = merge([
      this.firstConversatinonsMessages$,
      this.messagesLoaded$,
    ]).pipe(mergeAll());
  }

  private createMessage(message: IMessage) {
    const messageComponent = this.messagesref.createComponent(MessageComponent);

    messageComponent.setInput('message', {
      member: message.member,
      message: message.message,
    } as IMessage);
  }

  sendMessage(text: string) {
    console.log(
      text,
      this.messageResponse?.conversationId,
      this.authService.logedUser
    );
    if (this.messageResponse?.conversationId) {
      this.chatService
        .sendMessage(
          this.messageResponse.conversationId,
          this.authService.logedUser,
          text
        )
        .subscribe(() => {
          this.text.nativeElement.value = '';
        });

      return;
    }

    this.chatService
      .createConversation(this.user, this.authService.logedUser, text)
      .subscribe((nc) => {
        this.text.nativeElement.value = '';
        console.log('nc', nc);
        this.chatService.notifyConversationCreated(nc);
        this.messageResponse = {
          conversationId: nc.conversationId,
          messages: [],
        };
        this.createMessage({ member: nc.hostUser.id!, message: text });
      });
  }

  newConversation(user: IUser) {
    this.user = user;
    if (this.messageResponse) {
      this.messageResponse.conversationId = '';
      this.messageResponse.messages = [];
    }
    this.messagesLoadedSub.next([]);
  }

  loadMessages(conversation: IConversationPh) {
    this.chatService
      .conversationMessages(conversation.id)
      .pipe(
        tap((r) => (this.messageResponse = r)),
        tap((r) => this.chatService.connectConversation(r.conversationId)),
        mergeMap((cm) => of(cm.messages))
      )
      .subscribe((m) => this.messagesLoadedSub.next(m));
  }
}
