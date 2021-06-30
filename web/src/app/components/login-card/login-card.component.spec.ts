import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { LoginCardComponent } from './login-card.component';

/**
 * Runs all test for the 'LoginCardComponent'
 */
describe('LoginCardComponent', () => {
  let component: LoginCardComponent;
  let fixture: ComponentFixture<LoginCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ LoginCardComponent ],
      imports: []
    }).compileComponents();

    fixture = TestBed.createComponent(LoginCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  /**
   * Tests that the creation of the component works.
   */
  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
