import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { LoginComponent } from './login/login.component';
import { UserComponent } from './user/user.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AvatarComponent } from './components/avatar/avatar.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { UserDirective } from './components/user/user.directive';

@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    LoginComponent,
    UserComponent,
    AvatarComponent,
    SearchBarComponent,
    UserDirective,
  ],
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterModule],
  exports: [
    HeaderComponent,
    FooterComponent,
    LoginComponent,
    UserComponent,
    AvatarComponent,
    SearchBarComponent,
    // UserComponent,
    UserDirective,
  ],
})
export class CoreModule {}
