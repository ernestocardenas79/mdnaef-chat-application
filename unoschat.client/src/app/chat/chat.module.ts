import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatComponent } from './chat/chat.component';
import { ChatRoutingModule } from './chat-routing.module';
import { ConversationPlaceHolderComponent } from './conversation-place-holder/conversation-place-holder.component';
import { MessageComponent } from './message/message.component';
import { ConversationComponent } from './conversation/conversation.component';
import { CoreModule } from "../core/core.module";



@NgModule({
    declarations: [
        ChatComponent,
        ConversationPlaceHolderComponent,
        MessageComponent,
        ConversationComponent
    ],
    imports: [
        CommonModule,
        ChatRoutingModule,
        CoreModule
    ]
})
export class ChatModule { }
