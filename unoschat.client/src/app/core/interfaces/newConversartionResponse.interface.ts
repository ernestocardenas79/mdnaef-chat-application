import { IUser } from './user.interface';

export interface INewConversationResponse {
  user: IUser;
  hostUser: IUser;
  conversationId: string;
}
