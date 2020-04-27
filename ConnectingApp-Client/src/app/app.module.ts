import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ErrorInterceptorProvider } from './_service/error.interceptor';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { JwtModule } from '@auth0/angular-jwt'

import { AppComponent } from './app.component';
import { ValueComponent } from './value/value.component';
import { NavbarComponent } from './navbar/navbar.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { UserListComponent } from './user-list/user-list.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';


// first we get the token from local storage
export function getToken() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    ValueComponent,
    NavbarComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MessagesComponent,
    UserListComponent,
    MemberCardComponent,
    MemberDetailComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    NgxGalleryModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: getToken, // then we pass that token to token getter
        whitelistedDomains: ['localhost:5000'], // this list contains the trusted domains which can request to our apis
        blacklistedRoutes: ['localhost:5000/api/auth'] // the routes for which we don't need to change the headers
      }
    })
  ],
  providers: [
    ErrorInterceptorProvider
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
