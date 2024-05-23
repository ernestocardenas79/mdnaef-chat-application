import { NgModule } from '@angular/core';
import { RouterModule, Routes, mapToCanActivate } from '@angular/router';
import { LoginComponent } from './core/login/login.component';
import { AuthGuard } from './core/guards/AuthGuard';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    loadChildren: () =>
      import('./authentication/authentication.module').then(
        (m) => m.AuthenticationModule
      ),
  },
  {
    path: 'chat/:nme',
    canActivate: mapToCanActivate([AuthGuard]),
    loadChildren: () => import('./chat/chat.module').then((m) => m.ChatModule),
  },
  {
    path: '**',
    redirectTo: 'chat',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { bindToComponentInputs: true })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
