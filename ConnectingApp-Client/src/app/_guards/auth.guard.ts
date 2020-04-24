import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_service/auth.service';
import { AlertifyService } from '../_service/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  // for loggedin, alert and navigation we need below services
  constructor(private authService: AuthService,
              private alertify: AlertifyService,
              private router: Router) {}

  canActivate(
    // next has the next state of the route
    // state has the current state of the route so we can get ids etc from it

    // as we only check if the user is logged in or not so we return only boolean and
    // we dont need to get the current and next state of the route

    // next: ActivatedRouteSnapshot,
    // state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    // return true;

  ): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }
    this.alertify.warning("You are not authorize to access this page! Please Sign in!");
    this.router.navigate(['/']);
    return false;
  }
  
}
