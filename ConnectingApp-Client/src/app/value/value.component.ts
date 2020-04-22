import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  values: any;
  constructor(public data: HttpClient) { }

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
}
