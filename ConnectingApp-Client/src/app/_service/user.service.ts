import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { User } from '../shared/user';

// to send the jwt token
// later we'll see the automatic way to send the token to server
//const httpOptions = {
//  headers: new HttpHeaders({
//    'Authorization': 'Bearer ' + localStorage.getItem('token')
//  })
//};
// now we need to send this as we've sent the token using jwt module

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // to get all the users
  getUsers(): Observable<User[]> {

    // now we'll pass the header in get method
    return this.http.get<User[]>(this.baseUrl + 'user');
  }

  // to get specific user
  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'user/' + id);
  }

  // to update the profile
  UpdateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'user/' + id, user);
  }
}
