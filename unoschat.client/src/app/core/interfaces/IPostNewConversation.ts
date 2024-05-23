import { IUser } from './user.interface';

export interface IPostNewConversation {
  user: IUser;
  loggedUser: IUser;
  message: string;
}
