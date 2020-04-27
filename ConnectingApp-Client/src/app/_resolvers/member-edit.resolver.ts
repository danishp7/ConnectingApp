import { User } from '../shared/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { UserService } from '../_service/user.service';
import { AlertifyService } from '../_service/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_service/auth.service';

@Injectable({
  providedIn: 'root'
})
export class MemberEditResolver implements Resolve<User> {
  constructor(private userService: UserService,
    private alertify: AlertifyService,
    private router: Router,
    private authService: AuthService // to get the data of logged in user
  ) { }

  // now we are using this method to retrieve the data of logged in user 
  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    return this.userService.getUser(this.authService.decodedToken.nameid)
      .pipe(
        catchError(error => {
          this.alertify.error('Problem occur in retrieving data');

          // we need to redirect to home otherwise it will be infinite loop between members and member-detail
          this.router.navigate(['/members']); 
          return of(null); // of is rxjs type. to return null observable
        })
      );
  }

  
}
