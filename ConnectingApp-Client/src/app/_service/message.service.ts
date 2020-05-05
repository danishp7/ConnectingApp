import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginatedResult } from '../shared/pagination';
import { Message } from '../shared/message';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }


  // get all the message
  getMessages(id: number, page?, itemsPerPage?, messageContainer?) {
    // create paginatedresult of messages[] type instance
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();

    // now add message container params to httpparams
    let params = new HttpParams();
    params = params.append('MessageContainer', messageContainer);

    // check if header have pagination values
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    // now get all the messages
    return this.http.get<Message[]>(this.baseUrl + 'user/' + id + '/messages', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body; // this will contain all the messages

          // now check if request has the pagination headers
          if (response.headers.get('Pagination') !== null) {
            // to convert string to json object
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }


  // to get the conversation between 2 users
  getConversation(id: number, recipientId: number) {
    return this.http.get<Message[]>(this.baseUrl + 'user/' + id + '/messages/thread/' + recipientId);
  }


  // to send the message
  sendMessage(id: number, message: Message) {
    return this.http.post(this.baseUrl + 'user/' + id + '/messages/', message);
  }

  // to delete a msg
  deletMessage(id: number /*message id*/, userId: number /*loggedin user id*/) {
    return this.http.post(this.baseUrl + 'user/' + userId + '/messages/' + id, {});
  }

  // mark as read functionality
  markAsRead(userId: number, messageId: number) {
    // as we are sending nothing so we simply subscribe it here
    return this.http.post(this.baseUrl + 'user/' + userId + '/messages/' + messageId + '/read', {}).subscribe();
  }
}
