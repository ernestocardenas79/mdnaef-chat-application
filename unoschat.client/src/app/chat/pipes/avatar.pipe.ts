import { Pipe, PipeTransform } from '@angular/core';
import { ChatService } from '../../core/services/chat.service';
import { IMessage } from '../../core/interfaces';

@Pipe({
  name: 'avatar',
})
export class AvatarPipe implements PipeTransform {
  constructor(private chatService: ChatService) {}
  transform({ member }: IMessage): string {
    return this.chatService.contacts.get(member)!.avatar!;
  }
}
