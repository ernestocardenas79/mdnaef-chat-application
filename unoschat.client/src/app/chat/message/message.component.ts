import { Component, Input } from '@angular/core';
import { IMessage } from '../../core/interfaces';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css',
})
export class MessageComponent {
  @Input() message!: IMessage;
}
