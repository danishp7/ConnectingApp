import { User } from '../shared/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { UserService } from '../_service/user.service';
import { AlertifyService } from '../_service/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MemberListResolver implements Resolve<User[]> {
  constructor(private userService: UserService, private alertify: AlertifyService, private router: Router) { }

  // now we are using this method to retrieve the data insteead of loadUser
  resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
    return this.userService.getUsers()
      .pipe(
        catchError(error => {
          this.alertify.error('Problem occur in retrieving users');

          // we need to redirect to home otherwise it will be infinite loop between members and member-detail
          this.router.navigate(['/home']); 
          return of(null); // of is rxjs type. to return null observable
        })
      );
  }

  
}
