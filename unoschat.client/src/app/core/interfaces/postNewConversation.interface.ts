import { IUser } from './user.interface';

export interface IPostNewConversation {
  user: IUser;
  hostUser: IUser;
  message: string;
}
