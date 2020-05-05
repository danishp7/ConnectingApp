import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { AlertifyService } from '../_service/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../shared/message';
import { AuthService } from '../_service/auth.service';
import { MessageService } from '../_service/message.service';

@Injectable({
  providedIn: 'root'
})
export class MessagesResolver implements Resolve<Message[]> {
  // set the default values on page loading
  pageNumber: number = 1;
  pageSize: number = 5;

  // for inbox, outbox or inread
  messageContainer = 'Unread';

  constructor(private messageService: MessageService,
    private alertify: AlertifyService, private router: Router, private authService: AuthService) { }

  // now we are using this method to retrieve the data insteead of loadUser
  resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
    return this.messageService.getMessages(this.authService.decodedToken.nameid, this.pageNumber, this.pageSize, this.messageContainer)
      .pipe(
        catchError(error => {
          this.alertify.error('Problem occur in retrieving messages');

          // we need to redirect to home otherwise it will be infinite loop between members and member-detail
          this.router.navigate(['/home']);
          return of(null); // of is rxjs type. to return null observable
        })
      );
  }


}
