import {
  Directive,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  Output,
} from '@angular/core';
import { IUser } from '../../interfaces';

@Directive({
  selector: '[app-user]',
})
export class UserDirective {
  @Input() user!: IUser;

  @Output() userSelected = new EventEmitter<IUser>();

  constructor(private elementoRef: ElementRef) {}

  @HostListener('click')
  onClick() {
    this.userSelected.emit(this.user);
  }
}
