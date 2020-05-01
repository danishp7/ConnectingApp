import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators'
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../environments/environment';
import { User } from '../shared/user';

@Injectable({
  providedIn: "root"
})
export class AuthService {
  constructor(public http: HttpClient) { }

  baseUrl = environment.apiUrl + 'auth/';

  // jwt helper to verify token is jwt and it is expired or not
  jwtHelper = new JwtHelperService();

  // to decode the token and get the user name
  decodedToken: any;

  // login method
  // as this method return observalble so we need to subscribe this method in our login call
  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(
        map((response: any) => {
          const user = response;
          if (user) {
            // we set the token in localStorage
            localStorage.setItem('token', user.token);

            // to decode the token, it will decode the token and get the data present in the header
            // we want the 'unique_name' property so that we can show on nav 
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            console.log(this.decodedToken.nameid);
          }
        })
      );
  }

  // register method
  // as this method return observalble so we need to subscribe this method in our register call
  register(user: User) {
    return this.http.post(this.baseUrl + 'register', user);
  }

  // to verify the token from local storage is expired or not
  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
