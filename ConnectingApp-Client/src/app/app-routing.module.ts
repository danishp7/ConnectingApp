import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { UserListComponent } from './user-list/user-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { ListsResolver } from './_resolvers/lists.resolver';


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
      // as we are using resolver so we need to specify it here
      // note that 'user' here must be matched at subscribing function in respective component
      { path: 'members', component: MemberListComponent, resolve: { users: MemberListResolver } },
      { path: 'messages', component: MessagesComponent },

      // send the member edit reoslver in this route
      // if user go to another url without saving changes then to prompt user we add canDeactivate
      { path: 'members/edit', component: MemberEditComponent, resolve: { user: MemberEditResolver }, canDeactivate: [PreventUnsavedChanges] },

      // as we are using resolver so we need to specify it here
      // note that 'user' here must be matched at subscribing function in respective component
      { path: 'members/:id', component: MemberDetailComponent, resolve: { user: MemberDetailResolver } },
      { path: 'userlist', component: UserListComponent, resolve: { users: ListsResolver } }
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
