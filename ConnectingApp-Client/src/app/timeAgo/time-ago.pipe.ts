import { Pipe, ChangeDetectorRef } from '@angular/core';
import parse from 'date-fns/parseISO';
import  formatDistanceToNow  from 'date-fns/formatDistanceToNow';
import { AsyncPipe } from '@angular/common';
import { timer, Observable } from 'rxjs';
import { tap, map, distinctUntilChanged, startWith } from 'rxjs/operators';

@Pipe({
  name: 'timeAgo',
  pure: false,
})
export class TimeAgoPipe extends AsyncPipe {

  private time: Date;
  private formatted$: Observable<string>;

  constructor(private cd: ChangeDetectorRef) {
    super(cd);

    this.formatted$ = timer(0, 1000).pipe(
      map(() => formatDistanceToNow(this.time, { addSuffix: true, includeSeconds: true })),
      distinctUntilChanged()
    );
  }

  public transform(value: any): any {
    this.time = parse(value);
    return super.transform(this.formatted$);
  }

}
