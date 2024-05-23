import { IUser } from './user.interface';

export interface IPostMessage {
  conversationId: string;
  message: string;
  member: string;
}
