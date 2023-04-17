import { test, expect } from '@playwright/test';

test('Test journeys list', async ({ page }) => {
  await page.goto('http://localhost:8000/');
  await page.getByRole('button', { name: 'open drawer' }).click();
  await page.getByRole('button', { name: 'Journeys' }).click();
  await page.locator('button').nth(1).click();
  await page.getByText('Count: 10');
  await page.getByRole('button', { name: 'Next page' }).click();
  await page.getByText('Count: 10');
  await page.getByPlaceholder('Search').click();
  await page.getByPlaceholder('Search').fill('pasila');
  await page.locator('#orderBy').selectOption('departureStationName');
  await page.locator('#orderDir').selectOption('desc');
  await page.locator('#minDistance').click();
  await page.locator('#minDistance').fill('10');
  await page.locator('#minDistance').press('Tab');
  await page.locator('#maxDistance').fill('100');
  await page.locator('#maxDistance').press('Tab');
  await page.locator('#minDuration').fill('10');
  await page.locator('#minDuration').press('Tab');
  await page.locator('#maxDuration').fill('100');
  await page.getByRole('button', { name: 'Search' }).click();
  await page.getByText('Count: 50');
  await page.getByRole('button', { name: 'Next page' }).click();
  await page.getByRole('button', { name: 'Reset' }).click();
  await page.getByText('Count: 10');
});