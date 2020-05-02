import { Component, OnInit } from '@angular/core';
import { User } from '../shared/user';
import { Pagination, PaginatedResult } from '../shared/pagination';
import { AuthService } from '../_service/auth.service';
import { UserService } from '../_service/user.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_service/alertify.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  likesParam: string;

  constructor(private authService: AuthService,
    private userService: UserService,
    private route: ActivatedRoute,
    private alertify: AlertifyService
  ) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });

    this.likesParam = 'Likees';

  }

  // to set the current page on click event
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;

    // now we need to load the respective chunk of users on clicking any of the number
    this.loadUsers();
  }

  loadUsers() {
    // we also need to add 3rd param in which age or gender etc
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null, this.likesParam)
      .subscribe((response: PaginatedResult<User[]>) => {
        this.users = response.result; // to get list of items
        this.pagination = response.pagination; // to get header values
      },
        error => {
          this.alertify.error(error);
        });
  }
}
