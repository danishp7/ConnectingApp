import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { UserListComponent } from './user-list/user-list.component';
import { AuthGuard } from './_guards/auth.guard';


const routes: Routes = [
  // here we define our paths
  // wildcard path will always be at bottom
  // angular works in top to bottom approach to read the paths

  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },

  // to protect the multiple routes we use dummy route whose path would be '' which points to ..../ 
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'members', component: MemberListComponent },
      { path: 'messages', component: MessagesComponent },
      { path: 'userlist', component: UserListComponent }
    ]
  },

  // wild route
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
