import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import {IonicModule, Platform} from '@ionic/angular';

import { RegisterCardComponent } from './register-card.component';

describe('RegisterCardComponent', () => {
  let component: RegisterCardComponent;
  let fixture: ComponentFixture<RegisterCardComponent>;

  const setBeforeEach=(imports, providers) =>{
    beforeEach(waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [ RegisterCardComponent ],
        imports,
        providers
      }).compileComponents();

      fixture = TestBed.createComponent(RegisterCardComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
    }));
  };

  describe('general', () => {

    setBeforeEach([IonicModule.forRoot()], []);

    /**
     * Tests that the creation of the component works
     */
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    /**
     * Tests if the values in the constants file are correctly set
     * in the HTML of the component.
     */
    it('should set values in constants', () => {
      component.showVerify = true;
      fixture.detectChanges();
      const fName = fixture.debugElement.nativeElement.querySelector('ion-label[name="firstName"]').innerHTML;
      expect(fName).toBe(component.constants.labels.firstName);

      const lName = fixture.debugElement.nativeElement.querySelector('ion-label[name="lastName"]').innerHTML;
      expect(lName).toBe(component.constants.labels.lastName);

      const email = fixture.debugElement.nativeElement.querySelector('ion-label[name="emailAddr"]').innerHTML;
      expect(email).toBe(component.constants.labels.emailAddress);

      const verify = fixture.debugElement.nativeElement.querySelector('ion-label[name="verifyCode"]').innerHTML;
      expect(verify).toBe(component.constants.labels.enterCode);

      const pWord = fixture.debugElement.nativeElement.querySelector('ion-label[name="password"]').innerHTML;
      expect(pWord).toBe(component.constants.labels.password);

      const pWordConf = fixture.debugElement.nativeElement.querySelector('ion-label[name="passwordConfirm"]').innerHTML;
      expect(pWordConf).toBe(component.constants.labels.passwordConfirm);

      const verifyBtn = fixture.debugElement.nativeElement.querySelector('ion-button[name="verifyBtn"]').innerHTML;
      expect(verifyBtn).toBe(component.constants.labels.verifyEmail);
    });
  });

  /**
   * Run test suits specific to the desktop version of the container
   */
  describe('desktop', () => {
    const mockPlatform = jasmine.createSpyObj('Platform', ['width']);
    mockPlatform.width.and.callFake(() =>701);

    setBeforeEach([IonicModule.forRoot()],[{provide: Platform, useValue: mockPlatform}]);

    /**
     * Tests the 'adjustSize' function. Since the platform's width is set to 701 px, it should just return
     * the value as is.
     */
    it('should adjust for desktop', () => {
      expect(component.adjustSize(6)).toBe('6');
      expect(component.adjustSize(12)).toBe('12');
      expect(component.adjustSize(15)).toBe('12');
      expect(component.adjustSize(0)).toBe('1');
      expect(component.adjustSize(-1)).toBe('1');
    });
  });

  describe('mobile', () => {
    const mockPlatform = jasmine.createSpyObj('Platform', ['width']);
    mockPlatform.width.and.callFake(() =>699);

    beforeEach(waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [ RegisterCardComponent ],
        imports: [IonicModule.forRoot()],
        providers: [
          {provide: Platform, useValue: mockPlatform} // provide our own mock object for ionic's Platform object
        ]
      }).compileComponents();

      fixture = TestBed.createComponent(RegisterCardComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
    }));

    /**
     * Tests the 'adjustSize' function. Since the platform's width is set to 701 px, it should just return
     * the value as is.
     */
    it('should adjust for mobile', () => {
      expect(component.adjustSize(6)).toBe('12');
      expect(component.adjustSize(5)).toBe('10');
      expect(component.adjustSize(11)).toBe('12');
      expect(component.adjustSize(12)).toBe('12');
      expect(component.adjustSize(0)).toBe('1');
      expect(component.adjustSize(-1)).toBe('1');
    });
  });


});
