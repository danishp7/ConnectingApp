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
export class MemberDetailResolver implements Resolve<User> {
  constructor(private userService: UserService, private alertify: AlertifyService, private router: Router) { }

  // now we are using this method to retrieve the data insteead of loadUser
  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    return this.userService.getUser(route.paramMap.get('id'))
      .pipe(
        catchError(error => {
          this.alertify.error('Problem occur in retrieving users');
          this.router.navigate(['/members']);
          return of(null); // of is rxjs type. to return null observable
        })
      );
  }

  
}
