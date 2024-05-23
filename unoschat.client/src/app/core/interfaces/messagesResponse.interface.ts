import { IMessage } from './message.interface';

export interface IMessagesResponse {
  id?: string;
  conversationId: string;
  messages: IMessage[];
}
