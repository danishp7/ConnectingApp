import { Component, OnInit } from '@angular/core';
import { MessageService } from '../_service/message.service';
import { AlertifyService } from '../_service/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Message } from '../shared/message';
import { Pagination, PaginatedResult } from '../shared/pagination';
import { AuthService } from '../_service/auth.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  messageContainer = 'Unread';

  constructor(private messageService: MessageService,
    private alertify: AlertifyService, private route: ActivatedRoute, private authService: AuthService) { }

  ngOnInit(): void {
    // get the data from resolver
    this.route.data.subscribe(data => {
      this.messages = data['messages'].result; // it'll have all the messages
      this.pagination = data['messages'].pagination; // it'll have pagination header
    });
  }

  // load all messages
  loadMessages() {
    this.messageService.getMessages(this.authService.decodedToken.nameid, this.pagination.currentPage,
      this.pagination.itemsPerPage, this.messageContainer)
      // return type is [aginated result of message[], cz we are return list of messages
      .subscribe((res: PaginatedResult<Message[]>) => {
        this.messages = res.result;
        this.pagination = res.pagination;
      }, error => {
          this.alertify.error(error);
      });
  }

  // to set the list if page changed in pagination
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }

  // to remove the msg from list
  deleteMessage(id: number) {
    // first ask from user if he/she is sure of that
    this.alertify.confirm('Are you sure you want to delete this message?', () => {
      this.messageService.deletMessage(id, this.authService.decodedToken.nameid)
        .subscribe(() => {
          // now we need to splice the message array
          // we're not actually deleting the msg we just remove from list
          // msg will be deleted if both user deletes that message
          this.messages.splice(this.messages.findIndex(i => i.id == id), 1);
          this.alertify.success('Message has been deleted');
        }, error => {
            this.alertify.error(error);
        });
    });
  }


}
