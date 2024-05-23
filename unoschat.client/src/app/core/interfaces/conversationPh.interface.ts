import { IUser } from "./user.interface";

export interface IConversationPh {
    id: string;
    members: IUser[]
}