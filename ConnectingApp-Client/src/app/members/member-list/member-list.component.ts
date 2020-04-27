import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_service/user.service';
import { User } from '../../shared/user';
import { AlertifyService } from '../../_service/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['../../shared/site.css']
})
export class MemberListComponent implements OnInit {
  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  users: User[];

  // now we don't need loadUsers method
  ngOnInit(): void {
    this.route.data.subscribe(data => {
      // 'users' must be matched with 'users' passed in route
      this.users = data['users'];
    });
  }

  //loadUsers() {
  //  this.userService.getUsers().subscribe((response: User[]) => {
  //    this.users = response;
  //  },
  //    error => {
  //      this.alertify.error(error);
  //    });
  //}

}
