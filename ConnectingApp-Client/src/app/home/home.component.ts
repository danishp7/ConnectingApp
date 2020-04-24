import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['../shared/site.css']
})
export class HomeComponent implements OnInit {
  isRegistered = false;
  values: any;
  constructor(private data: HttpClient) { }

  ngOnInit(): void {
    this.getData();
  }
  getData() {
    this.data.get('http://localhost:5000/api/weatherforecast').subscribe(response => {
      this.values = response;
    },
      error => {
        console.log(error);
      });
  }

  IsRegistered() {
    this.isRegistered = true;
  }

  // to receive data from child component
  cancelRegistration(isRegistration: boolean) {
    this.isRegistered = isRegistration;
  }

}
