import { NgModule } from '@angular/core';
import { RouterModule, Routes, mapToCanActivate } from '@angular/router';
import { ChatComponent } from './chat/chat.component';
import { AuthGuard } from '../core/guards/AuthGuard';

const routes: Routes = [
  {
   path:'',
   canActivate: mapToCanActivate([AuthGuard]),
   component: ChatComponent,
  }
 ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatRoutingModule { }
