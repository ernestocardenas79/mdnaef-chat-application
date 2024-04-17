import { Injectable } from '@angular/core';
import { IUser } from '../../authentication/register/register.component';
import { ILogin } from '../login/login.component';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor() { }

  registerUser(user: IUser){
    console.log({user});
  }

   loginUser(user: ILogin){

  }
}
