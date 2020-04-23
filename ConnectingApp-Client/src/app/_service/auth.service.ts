import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators'
 
@Injectable({
  providedIn: "root"
})
export class AuthService {
  constructor(public http: HttpClient) { }

  baseUrl: string = "http://localhost:5000/api/auth/";

  // login method
  // as this method return observalble so we need to subscribe this method in our login call
  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(
        map((response: any) => {
          const user = response;
          if (user) {
            localStorage.setItem('token', user.token);
          }
        })
      );
  }

  // register method
  // as this method return observalble so we need to subscribe this method in our register call
  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }
}
