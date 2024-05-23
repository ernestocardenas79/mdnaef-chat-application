import { Component, Input, ViewChild, ViewContainerRef } from '@angular/core';
import {
  IConversationPh,
  INewConversationResponse,
} from '../../core/interfaces';
import { IUser } from '../../core/interfaces/user.interface';
import { ChatService } from '../../core/services/chat.service';
import { ConversationPlaceHolderComponent } from '../conversation-place-holder/conversation-place-holder.component';
import { ConversationComponent } from '../conversation/conversation.component';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css',
})
export class ChatComponent {
  conversationsPlaceHolder$ = this.chatService.conversationsPlaceHolder$;

  @Input('nme')
  set nme(u: string) {
    this.chatService.testAuthenticationPurpose(u);
  }

  @ViewChild('conversation') conversation!: ConversationComponent;
  @ViewChild('phHolder', { read: ViewContainerRef }) vf!: ViewContainerRef;
  constructor(private chatService: ChatService) {
    this.chatService.newConversation$.subscribe((nc) => this.createPhMan(nc));
  }

  newConversation(user: IUser) {
    console.log({ user });
    this.conversation.newConversation(user);
  }

  createPhMan(newConversation: INewConversationResponse) {
    const conversation = this.vf.createComponent(
      ConversationPlaceHolderComponent
    );

    conversation.setInput('conversationsPh', {
      id: newConversation.conversationId,
      members: [
        {
          email: newConversation.hostUser.email,
          name: newConversation.hostUser.name,
          avatar: newConversation.hostUser.avatar,
        },
      ],
    } as IConversationPh);
  }

  loadMessages(conversation: IConversationPh) {
    this.conversation.loadMessages(conversation);
    this.chatService.connectConversation(conversation.id);
  }
}
