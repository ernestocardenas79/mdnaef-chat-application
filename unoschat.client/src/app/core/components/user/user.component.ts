import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IUser } from '../../interfaces';

@Component({
  selector: '[app-user]',
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
})
export class UserComponent {
  @Input() user!: IUser;

  @Output() userSelected = new EventEmitter<IUser>();

  selectUser() {
    this.userSelected.emit(this.user);
  }
}
