import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_service/user.service';
import { User } from '../../shared/user';
import { AlertifyService } from '../../_service/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from '../../shared/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['../../shared/site.css']
})
export class MemberListComponent implements OnInit {
  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  users: User[];
  pagination: Pagination; // to get the header values

  // now we don't need loadUsers method
  ngOnInit(): void {
    this.route.data.subscribe(data => {
      // 'users' must be matched with 'users' passed in route

      // as we have now 2 info 1: list of items and 2: pagination so
      this.users = data['users'].result; // this will contain the list if items
      this.pagination = data['users'].pagination; // this will have the header values
    });
  }

  // to set the current page on click event
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;

    // now we need to load the respective chunk of users on clicking any of the number
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe((response: PaginatedResult<User[]>) => {
        this.users = response.result; // to get list of items
        this.pagination = response.pagination; // to get header values
    },
      error => {
        this.alertify.error(error);
      });
  }

}
