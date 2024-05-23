import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, filter } from 'rxjs';
import { environment } from '../../../environment/environment';
import { IAuthResponse, IAuthUser } from '../interfaces';
import { ILogin } from '../interfaces/login.Interface';
import { IUser } from '../interfaces/user.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private authenticationBehaiviourSub = new BehaviorSubject<IUser | undefined>( //{
    //   email: 'ernesto@cardenas.com',
    //   user: 'ernesto@cardenas.com',
    //   name: 'Ernesto',
    //   id: '663bcbdb451620cc678b1101',
    //   avatar: 'https://i.pravatar.cc/40?u=ernesto@cardenas.comErnesto',
    // }
    undefined
  );

  authentication$ = this.authenticationBehaiviourSub.asObservable();

  userAuthenticated$ = this.authentication$.pipe(
    filter((u) => u !== undefined)
  );

  logedUser!: IUser;

  constructor(
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService
  ) {
    this.userAuthenticated$.subscribe((u) => {
      if (u) {
        this.logedUser = u;
        this.logedUser.user = u.user;
      }
    });
  }

  registerUser(user: IAuthUser) {
    return this.httpClient.post<IAuthResponse>(
      `${environment.urlAuth}/api/Account/Registration`,
      user
    );
  }

  loginUser(user: ILogin) {
    return this.httpClient.post<IAuthResponse>(
      `${environment.urlAuth}/api/Account/Login`,
      user
    );
  }

  logoutUser() {
    this.authenticationBehaiviourSub.next(undefined);
  }

  get isAuthenticated() {
    return this.token && !this.jwtHelper.isTokenExpired(this.token);
  }

  get token() {
    return localStorage.getItem('token');
  }

  setAuthChange(userAuthenticated: IUser) {
    this.authenticationBehaiviourSub.next(userAuthenticated);
  }

  public userInformation(email: string) {
    return this.httpClient.get<IUser>(
      `${environment.urlChat}/api/user/${email}`
    );
  }
}
