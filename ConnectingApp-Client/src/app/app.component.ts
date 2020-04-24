import { Component, OnInit } from '@angular/core';
import { AuthService } from './_service/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  constructor(private service: AuthService) { }
  jwtHelper = new JwtHelperService();

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (token) {
      this.service.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }
}
