import { Component, OnInit } from '@angular/core';
import { auth } from '../shared/auth';
import { AuthService } from '../_service/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  model: auth = new auth();
  constructor(private service: AuthService) { }

  ngOnInit(): void {
  }

  login() {
    this.service.login(this.model).subscribe(response => {
      console.log("login successfully");
    }, error => {
        console.log(error);
    });
  }

  IsLoggedIn() {
    // first we get the token from localstorage
    const token = localStorage.getItem('token');

    // to return whether or not token value is true or it has some value
    return !!token;
  }

  logout() {
    // in logout we also remove token from local storage
    localStorage.removeItem('token');
    console.log("logged out");
  }
}
