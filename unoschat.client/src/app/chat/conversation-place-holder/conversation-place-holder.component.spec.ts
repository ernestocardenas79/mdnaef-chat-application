import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConversationPlaceHolderComponent } from './conversation-place-holder.component';

describe('ConversationPlaceHolderComponent', () => {
  let component: ConversationPlaceHolderComponent;
  let fixture: ComponentFixture<ConversationPlaceHolderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ConversationPlaceHolderComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConversationPlaceHolderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
