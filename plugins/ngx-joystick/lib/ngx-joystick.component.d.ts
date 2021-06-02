import { OnInit, EventEmitter, ElementRef, OnDestroy } from '@angular/core';
import * as nipplejs from 'nipplejs';
import * as ɵngcc0 from '@angular/core';
export interface JoystickEvent {
    event: nipplejs.EventData;
    data: nipplejs.JoystickOutputData;
}
export declare class NgxJoystickComponent implements OnInit, OnDestroy {
    private el;
    options: nipplejs.JoystickManagerOptions;
    move: EventEmitter<JoystickEvent>;
    start: EventEmitter<JoystickEvent>;
    end: EventEmitter<JoystickEvent>;
    dir: EventEmitter<JoystickEvent>;
    dirUp: EventEmitter<JoystickEvent>;
    dirDown: EventEmitter<JoystickEvent>;
    dirLeft: EventEmitter<JoystickEvent>;
    dirRight: EventEmitter<JoystickEvent>;
    plain: EventEmitter<JoystickEvent>;
    plainUp: EventEmitter<JoystickEvent>;
    plainDown: EventEmitter<JoystickEvent>;
    plainLeft: EventEmitter<JoystickEvent>;
    plainRight: EventEmitter<JoystickEvent>;
    manager: nipplejs.JoystickManager;
    constructor(el: ElementRef);
    ngOnInit(): void;
    ngOnDestroy(): void;
    private getDefaultOptions;
    private emitEvent;
    private setupEvents;
    static ɵfac: ɵngcc0.ɵɵFactoryDeclaration<NgxJoystickComponent, never>;
    static ɵcmp: ɵngcc0.ɵɵComponentDeclaration<NgxJoystickComponent, "ngx-joystick", never, { "options": "options"; }, { "move": "move"; "start": "start"; "end": "end"; "dir": "dir"; "dirUp": "dirUp"; "dirDown": "dirDown"; "dirLeft": "dirLeft"; "dirRight": "dirRight"; "plain": "plain"; "plainUp": "plainUp"; "plainDown": "plainDown"; "plainLeft": "plainLeft"; "plainRight": "plainRight"; }, never, never>;
}

//# sourceMappingURL=ngx-joystick.component.d.ts.map