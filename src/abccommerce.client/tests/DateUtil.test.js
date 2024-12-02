import { describe, it, expect } from 'vitest';
import { DateUtil } from '../src/util/date';
describe('DateUtil class test', () => {
    describe('getDateText', () => {
        it('should return "Just now" for a date that is less than a minute ago', () => {
            const ms = Date.now() - 50000; // now - 50 seconds
            const date = new Date(ms);
            const result = DateUtil.getDateText(date);
            expect(result).toBe('Just now');
        })

        it('should return 1 minute ago for a date that is only 1 minute ago', () => {
            const ms = Date.now() - (60000); // now - 1 minute
            const date = new Date(ms);
            const result = DateUtil.getDateText(date);
            expect(result).toBe('1 minute ago');
        })

        it('should return 5 minutes ago for a date that is 5 minutes ago', () => {
            const ms = Date.now() - (60000 * 5); // now - 5 minutes
            const date = new Date(ms);
            const result = DateUtil.getDateText(date);
            expect(result).toBe('5 minutes ago');
        })


        it('should return 1 hour ago for a date that is only 1 hour ago', () => {
            const ms = Date.now() - (60000 * 60); // now - 1 hour
            const date = new Date(ms);
            const result = DateUtil.getDateText(date);
            expect(result).toBe('1 hour ago');
        })

        it('should return 5 hours ago for a date that is 5 hours ago', () => {
            const ms = Date.now() - (60000 * 60 * 5); // now - 5 hours
            const date = new Date(ms);
            const result = DateUtil.getDateText(date);
            expect(result).toBe('5 hours ago');
        })


        it('should return 1 day ago for a date that is only 1 day ago', () => {
            const ms = Date.now() - (60000 * 60 * 24); // now - 1 day
            const date = new Date(ms);
            const result = DateUtil.getDateText(date);
            expect(result).toBe('1 day ago');
        })

        it('should return 5 days ago for a date that is 5 days ago', () => {
            const ms = Date.now() - (60000 * 60 * 24 * 5); // now - 5 days
            const date = new Date(ms);
            const result = DateUtil.getDateText(date);
            expect(result).toBe('5 days ago');
        })


        it('should return 1 month ago for a date that is only 1 month ago', () => {
            const ms = Date.now() - (60000 * 60 * 24 * 31); // now - 1 month (31 days)
            const date = new Date(ms);
            const result = DateUtil.getDateText(date);
            expect(result).toBe('1 month ago');
        })

        it('should return 5 months ago for a date that is 5 months ago', () => {
            const ms = Date.now() - (60000 * 60 * 24 * 5 * 31); // now - 5 months (155 days)
            const date = new Date(ms);
            const result = DateUtil.getDateText(date);
            expect(result).toBe('5 months ago');
        })


        it('should return 1 year ago for a date that is only 1 year ago', () => {
            const ms = Date.now() - (60000 * 60 * 24 * 365.25); // now - 1 year
            const date = new Date(ms);
            const result = DateUtil.getDateText(date);
            expect(result).toBe('1 year ago');
        })

        it('should return 5 years ago for a date that is 5 years ago', () => {
            const ms = Date.now() - (60000 * 60 * 24 * 5 * 365.25); // now - 5 years
            const date = new Date(ms);
            const result = DateUtil.getDateText(date);
            expect(result).toBe('5 years ago');
        })
    })
    
    describe('monthsSince', () => {
        it('should return <= 0 for a date in the same current month', () => {
            const currentDate = new Date();
            const sameMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() > 1 ? currentDate.getDate() - 1: currentDate.getDate());
            const result = DateUtil.monthsSince(sameMonth);
            expect(result).lessThanOrEqual(0);
        })
        
        it('should return <= 0 for a date on the previous month of the following day', () => {
            const currentDate = new Date();
            const underMonthAgo = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1);
            const result = DateUtil.monthsSince(underMonthAgo);
            expect(result).lessThanOrEqual(0);
        })

        it('should return 1 for a date on the previous month of the same day', () => {
            const currentDate = new Date();
            const monthAgo = new Date(currentDate.getFullYear(), currentDate.getMonth() - 1);
            const result = DateUtil.monthsSince(monthAgo);
            expect(result).toBe(1);
        })

        it('should return 1 for a date on the previous month of the preceding day', () => {
            const currentDate = new Date();
            const monthAgo = new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, currentDate.getDate() - 1);
            const result = DateUtil.monthsSince(monthAgo);
            expect(result).toBe(1);
        })

    })
})