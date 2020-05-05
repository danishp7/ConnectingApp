import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_service/user.service';
import { User } from '../../shared/user';
import { AlertifyService } from '../../_service/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from '../../shared/pagination';
import { AuthService } from '../../_service/auth.service';


@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['../../shared/site.css']
})
export class MemberListComponent implements OnInit {
  constructor(private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
  ) { }

  users: User[];
  pagination: Pagination; // to get the header values

  // to set the age gender and id filters
  // first store the user in localstorage so that we can get the user infos
  // user: User = JSON.parse(localStorage.getItem('token'));
  

  // now for the select option list in html to select gender option
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }];
  user: User = JSON.parse(localStorage.getItem('user'));
  gender: string = localStorage.getItem('body');

  // for age, gender
  // this willbe set in onit and then set it as params
  userParams: any = {};

  // now we don't need loadUsers method
  ngOnInit(): void {
    this.route.data.subscribe(data => {
      // 'users' must be matched with 'users' passed in route

      // as we have now 2 info 1: list of items and 2: pagination so
      this.users = data['users'].result; // this will contain the list if items
      this.pagination = data['users'].pagination; // this will have the header values
    });

    this.userParams.minAge = 7;
    this.userParams.maxAge = 99;
    this.userParams.gender = this.gender === 'female' ? 'male' : 'female';
    this.userParams.orderBy = 'lastActive';
  }

  // to reset all the filters means set all values back to default values and show all the list of users
  resetFilters() {
    // to set age gender
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 7;
    this.userParams.maxAge = 99;
    

    // to show the all list of users
    this.loadUsers();
  }


  // to set the current page on click event
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;

    // now we need to load the respective chunk of users on clicking any of the number
    this.loadUsers();
  }

  loadUsers() {
    // we also need to add 3rd param in which age or gender etc
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe((response: PaginatedResult<User[]>) => {
        this.users = response.result; // to get list of items
        this.pagination = response.pagination; // to get header values
    },
      error => {
        this.alertify.error(error);
      });
  }

}
