import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { WelcomeCardComponent } from './welcome-card.component';

describe('WelcomeCardComponent', () => {
  let component: WelcomeCardComponent;
  let fixture: ComponentFixture<WelcomeCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ WelcomeCardComponent ],
      imports: []
    }).compileComponents();

    fixture = TestBed.createComponent(WelcomeCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
