import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { VideostreamCardComponent } from './videostream-card.component';

/**
 * Runs all tests for the 'VideostreamCardComponent'
 */
describe('VideostreamCardComponent', () => {
  let component: VideostreamCardComponent;
  let fixture: ComponentFixture<VideostreamCardComponent>;

  /**
   * Boilerplate code to run before each test. It configures and creates the component.
   */
  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ VideostreamCardComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(VideostreamCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  /**
   * Tests that creating the component works
   */
  it('should create', () => {
    expect(component).toBeTruthy();
  });

  /**
   * Tests that the click of the 'close' button calls the 'dismissModal' function
   */
  it('should dismiss', () => {
    spyOn(component, 'dismissModal');
    const btn = fixture.debugElement.nativeElement.querySelector('ion-button[name="dismissBtn"]');
    btn.click();
    expect(component.dismissModal).toHaveBeenCalled()
  })
});
