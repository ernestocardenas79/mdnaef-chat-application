import { Component, Input } from '@angular/core';
import { IConversationPh } from '../../core/interfaces';

@Component({
  selector: 'app-conversation-place-holder',
  templateUrl: './conversation-place-holder.component.html',
  styleUrl: './conversation-place-holder.component.css',
})
export class ConversationPlaceHolderComponent {
  @Input() conversationsPh!: IConversationPh;
}
