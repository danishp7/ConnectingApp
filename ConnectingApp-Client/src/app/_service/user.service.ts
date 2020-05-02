import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { User } from '../shared/user';
import { PaginatedResult } from '../shared/pagination';
import { map } from 'rxjs/operators';

// to send the jwt token
// later we'll see the automatic way to send the token to server
//const httpOptions = {
//  headers: new HttpHeaders({
//    'Authorization': 'Bearer ' + localStorage.getItem('token')
//  })
//};
// now we need to send this as we've sent the token using jwt module

const headerValues = {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
  'Access-Control-Allow-Headers': 'Content-Type'
};

const headerOptions = {
  headers: new Headers(headerValues)
};

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // to get all the users we need to send the pagenumber and page size as optional parameters in the func
  getUsers(page?, itemsPerPage?, userParams?/* for age or gender filter*/, likesParam? /*for likes param i.e likers=true or likee=true*/): Observable<PaginatedResult<User[]>> {

    // first we need to store pagination values
    const pagination: PaginatedResult<User[]> = new PaginatedResult<User[]>();

    // 2nd we have to sent httpparams in the method 
    let params = new HttpParams();

    // now we check if the parameters are not null then we append them to our paramas
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    // we also check for the user params if they are null or not
    // if user enter any value for age and gender etc
    // if yes then we will set that values
    if (userParams != null) {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);

    }

    // to check what is inside likeparams
    // likeParams ki default value ko hm ListResolver me set karen ge
    if (likesParam === 'Likers') {
      params = params.append('likers', 'true');
    }

    if (likesParam === 'Likees') {
      params = params.append('likees', 'true');
    }

    //now we send these params into method

    return this.http.get<User[]>(this.baseUrl + 'user', { observe: 'response', params })
      .pipe(
        // we need to store the respone cz in response we have pagination and list of items
        map(response => {
          pagination.result = response.body; // the body contain the list of items

          // now we need to set the headers
          // first we check if the header value is null or not
          if (response.headers.get('Pagination') != null) {
            // to convert the serialize string back to json onject we use json.parse
            pagination.pagination = JSON.parse(response.headers.get('Pagination')); 
          }
          return pagination;
        })
    );

    // now we need to edit the member list resolver and memberlist.component.ts
  }

  // to get specific user
  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'user/' + id);
  }

  // to update the profile
  UpdateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'user/' + id, user);
  }

  

  // to like someone
  sendLike(id: number, recipientId: number) {
    return this.http.post(this.baseUrl + 'user/' + id + '/like/' + recipientId, headerOptions);
  }


}
