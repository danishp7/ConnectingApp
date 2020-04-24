import { Component, OnInit } from '@angular/core';
import { auth } from '../shared/auth';
import { AuthService } from '../_service/auth.service';
import { AlertifyService } from '../_service/alertify.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['../shared/site.css']
})
export class NavbarComponent implements OnInit {
  model: auth = new auth();

  constructor(public service: AuthService, private alertify: AlertifyService) { }

  ngOnInit(): void {
  }

  login() {
    this.service.login(this.model).subscribe(response => {
      this.alertify.success("login successfully");
    }, error => {
        this.alertify.error(error);
        
    });
  }

  IsLoggedIn() {
    // first we get the token from localstorage
    // const token = localStorage.getItem('token');

    // to return whether or not token value is true or it has some value
    // return !!token;

    // now we use the jwthelper which also check that our token is expired or not so we use below code
    return this.service.loggedIn();
  }

  logout() {
    // in logout we also remove token from local storage
    localStorage.removeItem('token');
    this.alertify.message("logged out");
  }
}
