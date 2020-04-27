import { Component, OnInit, Input } from '@angular/core';
import { User } from '../../shared/user';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['../../shared/site.css']
})
export class MemberCardComponent implements OnInit {

  // to get the input from parent (member-list) component
  @Input() userFromMemberList: User;
  constructor() { }

  ngOnInit(): void {
  }

}
