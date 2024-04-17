import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './core/login/login.component';

const routes: Routes = [
  {
    path: '',
    pathMatch:'full',
    loadChildren:()=> import('./chat/chat.module').then(m=> m.ChatModule)
  },
  {
  path:'login',
  component: LoginComponent
},
{
  path:'register',
  loadChildren: ()=> import('./authentication/authentication.module').then(m=>m.AuthenticationModule)
},
{
  path:'chat',
  loadChildren: ()=> import('./chat/chat.module').then(m=> m.ChatModule)
},
{
  path:'**',
  redirectTo: 'chat'
}];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
