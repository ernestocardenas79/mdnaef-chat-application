import {
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Output,
  Renderer2,
  ViewChild,
} from '@angular/core';
import {
  Observable,
  Subject,
  debounceTime,
  filter,
  fromEvent,
  map,
  merge,
  mergeAll,
  mergeMap,
} from 'rxjs';
import { IUser } from '../../interfaces';
import { ChatService } from '../../services/chat.service';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrl: './search-bar.component.css',
})
export class SearchBarComponent {
  @ViewChild('searchInput') searchInput!: ElementRef;
  @ViewChild('results', { static: true }) resultsEl!: ElementRef;
  @Output() selectedUser = new EventEmitter<IUser>();
  search$!: Observable<IUser[]>;
  criteriaFilter$!: Observable<InputEvent>;
  cleanResults$!: Observable<undefined>;
  results$!: Observable<IUser[] | undefined>;
  selectedUserSub = new Subject<undefined>();
  selectedUser$ = this.selectedUserSub.asObservable();

  constructor(private chatService: ChatService, private renderer: Renderer2) {}

  ngAfterViewInit(): void {
    this.criteriaFilter$ = fromEvent<InputEvent>(
      this.searchInput.nativeElement,
      'input'
    );

    this.search$ = this.criteriaFilter$.pipe(
      debounceTime(300),
      filter((v) => (v.target! as HTMLInputElement).value !== ''),
      map((v) => (v.target! as HTMLInputElement).value),
      mergeMap((u) => this.chatService.findUser(u)),
      filter((u) => u.length > 0)
    );

    this.cleanResults$ = this.criteriaFilter$.pipe(
      filter((v) => (v.target! as HTMLInputElement).value === ''),
      map((_) => undefined)
    );

    this.results$ = merge([
      this.search$,
      this.cleanResults$,
      this.selectedUser$,
    ]).pipe(mergeAll());
  }

  selectUser(user: IUser) {
    console.log('selectedUser');
    this.selectedUser.emit(user);
    this.selectedUserSub.next(undefined);
    this.searchInput.nativeElement.value = '';
  }

  focus() {
    this.renderer.removeClass(this.resultsEl.nativeElement, 'lost-focus');
  }

  @HostListener('mouseleave', ['$event'])
  onBlur(e: PointerEvent) {
    e.preventDefault();
    if (this.resultsEl) {
      this.renderer.addClass(this.resultsEl.nativeElement, 'lost-focus');
    }
  }

  @HostListener('mouseenter', ['$event'])
  onMouseEnter(e: PointerEvent) {
    e.preventDefault();
    if (this.resultsEl) {
      this.renderer.removeClass(this.resultsEl.nativeElement, 'lost-focus');
    }
  }
}
